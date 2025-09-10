#!/bin/bash
$1 -e $(dirname "$0")/call_proc_2.sh $$ ${@:2}
status=$(</tmp/nsmbe_call_exit_$$.status)
rm -f /tmp/nsmbe_call_exit_$$.status
exit $status
