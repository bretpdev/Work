#UTLWG85.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG85.LWG85RZ
   then
        rm ${reportdir}/ULWG85.LWG85RZ
fi
if test -a ${reportdir}/ULWG85.LWG85R1
   then
        rm ${reportdir}/ULWG85.LWG85R1
fi
if test -a ${reportdir}/ULWG85.LWG85R2
   then
        rm ${reportdir}/ULWG85.LWG85R2
fi

# run the program

sas ${codedir}/UTLWG85.sas -log ${reportdir}/ULWG85.LWG85R1  -mautosource
