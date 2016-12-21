#!/bin/bash
set -eu
error() {
    echo "E: ${1}" >&2
    exit 1
}

if [ $(id -u) -eq 0 ]; then
    error "won't run as root"
fi

param=${1-undef}

jsvc=/usr/bin/jsvc

app_name="JewishBot"
app_home="${HOME}/jewish_bot"

pid_file="${app_home}/logs/app.pid"
std_err_file="${app_home}/logs/std-err.log"

exe="${app_home}/publish/JewishBot"

run_bot() {
    if [[ ! -f ${exe} ]]; then
        error "exec file not found at ${exe}"
    fi
    ${exe} &
    echo $!>${pid_file}
}

run_bot_stop() {
    if [[ -f ${pid_file} ]]; then
        kill `cat ${pid_file}`
        rm ${pid_file}
    else
        error "no pidfile found try kill manually"
    fi
}

is_running() {
    if [ -e ${pid_file} ]; then
      echo ${app_name} is running, pid=`cat ${pid_file}`
      return 0
   else
      echo ${app_name} is NOT running
      return 1
   fi
}

do_start() {
    if is_running; then
        error "${app_name} is already started"
    else
        echo -n "starting ${app_name} ..."
        run_bot ""
        sleep 1
        if is_running; then
            echo "done"
        else
            echo ""
            error "startup failed"
        fi
    fi
}

do_stop() {
    if is_running; then
        echo -n "stopping ${app_name} ... "
        run_bot_stop
        echo "done"
    else
        error "${app_name} is already stoped"
    fi
}

case "$param" in
    start)
        do_start
        ;;
    stop)
        do_stop
        ;;
    status)
        is_running
        ;;
    restart)
        echo "restarting ${app_name}:"
        if is_running; then
            do_stop
        fi
        do_start
        ;;
    *)
        echo "usage: daemon {start|stop|restart|status}" >&2
        exit 3
        ;;
esac
