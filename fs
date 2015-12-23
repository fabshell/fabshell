#!/usr/bin/env python

import os
import re
import sys
import shlex
import pprint
import readline
from cmd import Cmd
from pprint import pprint
from subprocess import Popen, check_call, check_output, PIPE, STDOUT, CalledProcessError

class FabShell(Cmd):

    def __init__(self, histfile=os.environ.get('HISTFILE', '~/.bash_history')):
        Cmd.__init__(self)
        self.prompt = 'fs> '

    def _load_history(self):
        histfile = os.environ['HISTFILE']
        cmds = open(os.path.expanduser(histfile)).read().split('\n')
        self.commands = [cmd for cmd in cmds if not cmd.startswith('#')]

    def emptyline(self):
        pass

    def do_EOF(self, line):
        print 'goodbye'
        return True

    def default(self, line):
        env = os.environ
        process = Popen(['/bin/bash', '--noprofile', '-i'], stdin=PIPE, stderr=PIPE, env=env)
        stdout, stderr = process.communicate(line)
        exitcode = process.poll()
        if exitcode:
            print 'stderr =', stderr
            self.prompt = str(exitcode) + ' fs> '
        else:
            self.prompt = 'fs> '
        return False

    def completenames(self, text, *ignored):
        matches = self.completedefault(text, *ignored)
        matches += Cmd.completenames(self, text, *ignored)
        return matches

    def completedefault(self, text, line, beg, end):
        self._load_history()
        return [cmd[beg:] for cmd in self.commands if cmd.startswith(line)]


if __name__ == '__main__':
    try:
        FabShell().cmdloop()
    except KeyboardInterrupt:
        print 'goodbye'
