#UTLWQ19.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ19.LWQ19R1
   then
        rm ${reportdir}/ULWQ19.LWQ19R1
fi
if test -a ${reportdir}/ULWQ19.LWQ19R2
   then
        rm ${reportdir}/ULWQ19.LWQ19R2
fi

# run the program

sas ${codedir}/UTLWQ19.sas -log ${reportdir}/ULWQ19.LWQ19R1  -mautosource
