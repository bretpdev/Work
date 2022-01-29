#UTLWQ20.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ20.LWQ20R1
   then
        rm ${reportdir}/ULWQ20.LWQ20R1
fi
if test -a ${reportdir}/ULWQ20.LWQ20R2
   then
        rm ${reportdir}/ULWQ20.LWQ20R2
fi

# run the program

sas ${codedir}/UTLWQ20.sas -log ${reportdir}/ULWQ20.LWQ20R1  -mautosource
