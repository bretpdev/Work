#UTLWG29.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG29.LWG29R1
   then
        rm ${reportdir}/ULWG29.LWG29R1
fi
if test -a ${reportdir}/ULWG29.LWG29R2
   then
        rm ${reportdir}/ULWG29.LWG29R2
fi
if test -a ${reportdir}/ULWG29.LWG29RZ
   then
        rm ${reportdir}/ULWG29.LWG29RZ
fi

# run the program

sas ${codedir}/UTLWG29.sas -log ${reportdir}/ULWG29.LWG29R1  -mautosource
