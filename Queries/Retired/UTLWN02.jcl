#UTLWN02.jcl  
#THE REPORT DIRECTORY FOR THIS JOB WAS HARDCODED PER THE REQUEST OF AES
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a /sas/whse/progrevw/ULWN02.LWN02R1
   then
        rm /sas/whse/progrevw/ULWN02.LWN02R1
fi
if test -a /sas/whse/progrevw/ULWN02.LWN02R2
   then
        rm /sas/whse/progrevw/ULWN02.LWN02R2
fi
if test -a /sas/whse/progrevw/ULWN02.LWN02RZ
   then
        rm /sas/whse/progrevw/ULWN02.LWN02RZ
fi

# run the program

sas ${codedir}/UTLWN02.sas -log /sas/whse/progrevw/ULWN02.LWN02R1  -mautosource
