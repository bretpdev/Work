#UTLWS21.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS21.LWS21R1
   then
        rm ${reportdir}/ULWS21.LWS21R1
fi
if test -a ${reportdir}/ULWS21.LWS21R2
   then
        rm ${reportdir}/ULWS21.LWS21R2
fi
if test -a ${reportdir}/ULWS21.LWS21RZ
   then
        rm ${reportdir}/ULWS21.LWS21RZ
fi

# run the program

sas ${codedir}/UTLWS21.sas -log ${reportdir}/ULWS21.LWS21R1  -mautosource
