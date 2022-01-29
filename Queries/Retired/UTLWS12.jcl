#UTLWS12.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS12.LWS12R1
   then
        rm ${reportdir}/ULWS12.LWS12R1
fi
if test -a /sas/whse/progrevw/ULWS12.LWS12R2
   then
        rm /sas/whse/progrevw/ULWS12.LWS12R2
fi
if test -a ${reportdir}/ULWS12.LWS12RZ
   then
        rm ${reportdir}/ULWS12.LWS12RZ
fi


# run the program

sas ${codedir}/UTLWS12.sas -log ${reportdir}/ULWS12.LWS12R1  -mautosource
