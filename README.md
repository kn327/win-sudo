# Win-Sudo #

A windows implementation of sudo inspired by the Linux sudo command (v1.0).

https://kn327.medium.com/win-sudo-the-struggle-for-admin-80f5895725e7

### Installation ###

To install, simply build the application and find the executable in the bin/Debug or bin/Release folder

Copy the executable to somewhere within your PATH env variable (optional).

### Usage ###

`sudo` Opens an elevated command shell in the current process.

`sudo [options] {command} [arguments]`

## [options] ##
# properties #
Properties appear in key value pair syntax `{property} {value}`
* `-stdout` will redirect the StdOut to the file specified (using this will prevent the stdout temp file from being deleted)
* `-stderr` will redirect the StdErr to the file specified (using this will prevent the stderr temp file from being deleted)
# flags #
* `--save-temp` will prevent the application from deleting the temp files after the process completed
* `--show-window` will display a new window for the elevated command prompt to allow you to write your own code manually, when this flag is enabled the output is not redirected anywhere.
* `--unescape-chars` will attempt to replace escaped characters with their unescaped counterparts so that the argument list is properly transcoded into cmd
* `--debug` will output debug lines detailing the commands being passed to the sub-process

### Examples ###

```
$> sudo --debug whoami /groups

$> sudo whoami /groups | find "12288" && echo elevated || echo not elevated

$> sudo --show-window cmd
```

### Who do I talk to? ###

* kom@ohmvision.com
