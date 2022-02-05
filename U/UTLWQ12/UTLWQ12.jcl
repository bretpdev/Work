#UTLWQ12.jcl  Accounts in Workgroup 15
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ12.LWQ12R1
   then
        rm ${reportdir}/ULWQ12.LWQ12R1
fi
if test -a ${reportdir}/ULWQ12.LWQ12R2
   then
        rm ${reportdir}/ULWQ12.LWQ12R2
fi

# run the program

sas ${codedir}/UTLWQ12.sas -log ${reportdir}/ULWQ12.LWQ12R1  -mautosource
