#UTLWG56.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG56.LWG56R1
   then
        rm ${reportdir}/ULWG56.LWG56R1
fi
if test -a ${reportdir}/ULWG56.LWG56R2
   then
        rm ${reportdir}/ULWG56.LWG56R2
fi
if test -a ${reportdir}/ULWG56.LWG56RZ
   then
        rm ${reportdir}/ULWG56.LWG56RZ
fi

# run the program

sas ${codedir}/UTLWG56.sas -log ${reportdir}/ULWG56.LWG56R1  -mautosource
