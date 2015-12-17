#!/bin/bash

set -x

mkdir -p ~/.pynet

curl -sL https://github.com/pynet/pynet/raw/master/pynet.py

curl_args=$(ps -eo args | grep curl | grep pynet | head -n 1)

url=$(echo $curl_args | sed -ne 's/.*\(http[^"]*\).*/\1/p')

exec ~/.pynet/pynet.py --pynet-url $url
