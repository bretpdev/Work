#UTLWG79.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG79.LWG79R1
   then
        rm ${reportdir}/ULWG79.LWG79R1
fi
if test -a ${reportdir}/ULWG79.LWG79R2
   then
        rm ${reportdir}/ULWG79.LWG79R2
fi
if test -a ${reportdir}/ULWG79.LWG79RZ
   then
        rm ${reportdir}/ULWG79.LWG79RZ
fi

# run the program

sas ${codedir}/UTLWG79.sas -log ${reportdir}/ULWG79.LWG79R1  -mautosource
