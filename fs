#!/usr/bin/env python

import os
import re
import sys
import shlex
import pprint
import readline
from cmd import Cmd
from pprint import pprint
from subprocess import check_call, check_output, PIPE

class FabShell(Cmd):

    def __init__(self, histfile=os.environ.get('HISTFILE', '~/.bash_history')):
        Cmd.__init__(self)
        self.prompt = 'fs> '
        self._load_history()
        self._cache_aliases()

    def _load_history(self):
        histfile = os.environ['HISTFILE']
        cmds = open(os.path.expanduser(histfile)).read().split('\n')
        self.commands = [cmd for cmd in cmds if not cmd.startswith('#')]

    def _cache_aliases(self):
        self.aliases = {}
        regex = re.compile("alias +([^ ]+)=['\"]([^'\"]+)['\"]")
        for line in check_output('/bin/bash -i -c alias', shell=True).split('\n'):
            match = regex.match(line)
            if match:
                key, value = match.groups()
                self.aliases[key] = value

    def emptyline(self):
        pass

    def default(self, line):
        args = [self.aliases.get(arg, arg) for arg in shlex.split(line)]
        return os.system(' '.join(args))

    def completenames(self, text, *ignored):
        matches = self.completedefault(text, *ignored)
        matches += Cmd.completenames(self, text, *ignored)
        return matches

    def completedefault(self, text, line, beg, end):
        return [cmd[beg:] for cmd in self.commands if cmd.startswith(line)]


if __name__ == '__main__':
    FabShell().cmdloop()
