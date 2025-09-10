#!/bin/bash
start_time=$(date +%s%3N)
${@:2}
status=$?
echo $status > /tmp/nsmbe_call_exit_$1.status

if [ $status -ne 0 ]; then
    read -n 1 -s -r -p "Press any key to continue..."
else
    duration=$(( $(date +%s%3N) - start_time ))
    min_duration=600
    remaining=$(( min_duration - duration ))

    if (( duration < min_duration )); then
        sleep "$(($remaining / 1000)).$(($(printf "%03d" $remaining) % 1000))"
    fi
fi

exit $status
