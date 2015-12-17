#!/bin/bash

set -x

mkdir -p ~/.pynet

curl -sL https://github.com/pynet/pynet/raw/master/pynet.py > ~/.pynet/pynet.py

curl_args=$(ps -eo args | grep curl | grep pynet | head -n 1)

url=$(echo $curl_args | sed -ne 's/.*\(http[^"]*\).*/\1/p')

exec python ~/.pynet/pynet.py --pynet-url $url