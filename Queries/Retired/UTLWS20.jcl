#UTLWS20.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS20.LWS20R1
   then
        rm ${reportdir}/ULWS20.LWS20R1
fi
if test -a ${reportdir}/ULWS20.LWS20R2
   then
        rm ${reportdir}/ULWS20.LWS20R2
fi
if test -a ${reportdir}/ULWS20.LWS20RZ
   then
        rm ${reportdir}/ULWS20.LWS20RZ
fi

# run the program

sas ${codedir}/UTLWS20.sas -log ${reportdir}/ULWS20.LWS20R1  -mautosource
