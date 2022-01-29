#UTLWQ01.jcl  Monthly Default Aversion Billings
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ01.LWQ01R1
   then
        rm ${reportdir}/ULWQ01.LWQ01R1
fi
if test -a ${reportdir}/ULWQ01.LWQ01R2
   then
        rm ${reportdir}/ULWQ01.LWQ01R2
fi

# run the program

sas ${codedir}/UTLWQ01.sas -log ${reportdir}/ULWQ01.LWQ01R1  -mautosource
