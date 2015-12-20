#!/usr/bin/env python

import os
import re
import sys
import cmd2
import shlex
import pprint
from subprocess import check_call, check_output, PIPE

class FabShell(cmd2.Cmd):
    def __init__(self):
        cmd2.Cmd.__init__(self)
        self.prompt = 'fs> '
        self.aliases = {}
        regex = re.compile("alias +([^ ]+)=['\"]([^'\"]+)['\"]")
        for line in check_output('/bin/bash -i -c alias', shell=True).split('\n'):
            match = regex.match(line)
            if match:
                key, value = match.groups()
                self.aliases[key] = value
    def default(self, line):
        print 'default', locals()
        args = shlex.split(line)
        args = [self.aliases.get(arg, arg) for arg in args]
        return os.system(' '.join(args))
    def completedefault(self, text, line, beg, end):
        print 'completedefault', locals()
        return []

if __name__ == '__main__':
    FabShell().cmdloop()
