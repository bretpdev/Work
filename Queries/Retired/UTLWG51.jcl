#UTLWG51.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG51.LWG51R1
   then
        rm ${reportdir}/ULWG51.LWG51R1
fi
if test -a ${reportdir}/ULWG51.LWG51R2
   then
        rm ${reportdir}/ULWG51.LWG51R2
fi
if test -a ${reportdir}/ULWG51.LWG51RZ
   then
        rm ${reportdir}/ULWG51.LWG51RZ
fi
# run the program

sas ${codedir}/UTLWG51.sas -log ${reportdir}/ULWG51.LWG51R1  -mautosource
