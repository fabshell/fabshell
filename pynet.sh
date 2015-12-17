#!/bin/bash

echo "pynet.sh:"

args="$@"

curl_args=$(ps -eo args | grep curl | grep pynet | head -n 1)

url=$(echo $curl_args | sed -ne 's/.*\(http[^"]*\).*/\1/p')

if [ -z "$url" ]; then
    echo "url not found using 'ps -eo args'"
    exit -1
fi

set -x

mkdir -p ~/.pynet

curl -sL https://github.com/pynet/pynet/raw/master/pynet.py > ~/.pynet/pynet.py

exec python ~/.pynet/pynet.py $url $args
