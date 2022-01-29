#UTLWE03.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE03.LWE03R1
   then
        rm ${reportdir}/ULWE03.LWE03R1
fi
if test -a ${reportdir}/ULWE03.LWE03R2
   then
        rm ${reportdir}/ULWE03.LWE03R2
fi
if test -a ${reportdir}/ULWE03.LWE03RZ
   then
        rm ${reportdir}/ULWE03.LWE03RZ
fi
# run the program

sas ${codedir}/UTLWE03.sas -log ${reportdir}/ULWE03.LWE03R1  -mautosource
