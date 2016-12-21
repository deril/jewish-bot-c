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
app_home="${HOME}/Projects/JewishBot/bin/release/netcoreapp1.1/osx.10.12-x64"

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
    else
        error "no pidfile found try kill manually"
    fi
}

do_ps_status() {
    pid=$1
    verbose=${2}
    ps_lines=$(ps -ae -o pid,ppid,uname,args \
                   | egrep --color=yes "\-procnam[e] ${app_name}" || true)
    pid_result=$(echo "$ps_lines" | egrep "^[ \t]*${pid}" || true)
    if [[ -z $pid_result ]]; then
        if [[ ${verbose} = 1 ]]; then
            echo "no process is running"
            if [[ -f ${pid_file} ]]; then
                pid_content="$(cat ${pid_file})"
                echo "pidfile content is: ${pid_content}"
                rm -vf ${pid_file}
            fi
        fi
        return 0
    else
        if [[ ${verbose} = 1 ]]; then
            echo "is running"
            echo -e "$pid_result"
        fi
        return 1
    fi

}

is_running() {
    verbose=${1-0}
    if [[ -f ${pid_file} ]]; then
        if [[ ${verbose} = 1 ]]; then
            echo "pidfile found"
        fi
        if do_ps_status $(cat ${pid_file}) ${verbose}; then
            return 1
        else
            return 0
        fi
    else
        if [[ ${verbose} = 1 ]]; then
            echo "no pidfile found"
        fi
        if do_ps_status "" ${verbose}; then
            return 1
        else
            return 0
        fi
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
        is_running 1
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
