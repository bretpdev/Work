#UTLWS16.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS16.LWS16R1
   then
        rm ${reportdir}/ULWS16.LWS16R1
fi
if test -a ${reportdir}/ULWS16.LWS16R2
   then
        rm ${reportdir}/ULWS16.LWS16R2
fi

# run the program

sas ${codedir}/UTLWS16.sas -log ${reportdir}/ULWS16.LWS16R1  -mautosource
