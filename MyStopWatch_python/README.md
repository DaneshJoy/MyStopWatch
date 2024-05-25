## Create Desktop App

Copy the ```MyStopWatch.desktop``` to ```~/.local/share/applications```



## Add to startup

Make the script executable in the properties window or by the following command:

```bash
chmod -x run.sh
```

Edit the crontab file:

```bash
crontab -e
```

Add the following line (change address if required) and reboot
```bash
@reboot  /home/saeed/Apps/MyStopwatch_python/run.sh
```

