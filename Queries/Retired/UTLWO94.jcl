#UTLWO94.jcl  Interest Penalities - Monthly
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO94.LWO94R1
   then
        rm ${reportdir}/ULWO94.LWO94R1
fi
if test -a ${reportdir}/ULWO94.LWO94R2
   then
        rm ${reportdir}/ULWO94.LWO94R2
fi
if test -a ${reportdir}/ULWO94.LWO94RZ
   then
        rm ${reportdir}/ULWO94.LWO94RZ
fi
# run the program

sas ${codedir}/UTLWO94.sas -log ${reportdir}/ULWO94.LWO94R1  -mautosource
