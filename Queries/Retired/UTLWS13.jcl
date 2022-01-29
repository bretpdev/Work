#UTLWS13.jcl  Skip Activity
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS13.LWS13R1
   then
        rm ${reportdir}/ULWS13.LWS13R1
fi
if test -a ${reportdir}/ULWS13.LWS13R2
   then
        rm ${reportdir}/ULWS13.LWS13R2
fi

# run the program

sas ${codedir}/UTLWS13.sas -log ${reportdir}/ULWS13.LWS13R1  -mautosource
