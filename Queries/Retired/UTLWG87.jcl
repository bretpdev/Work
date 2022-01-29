#UTLWG87.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG87.LWG87R1
   then
        rm ${reportdir}/ULWG87.LWG87R1
fi
if test -a ${reportdir}/ULWG87.LWG87R2
   then
        rm ${reportdir}/ULWG87.LWG87R2
fi
if test -a ${reportdir}/ULWG87.LWG87R3
   then
        rm ${reportdir}/ULWG87.LWG87R3
fi
if test -a ${reportdir}/ULWG87.LWG87R4
   then
        rm ${reportdir}/ULWG87.LWG87R4
fi
if test -a ${reportdir}/ULWG87.LWG87RZ
   then
        rm ${reportdir}/ULWG87.LWG87RZ
fi
# run the program

sas ${codedir}/UTLWG87.sas -log ${reportdir}/ULWG87.LWG87R1  -mautosource
