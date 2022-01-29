#UTLWG83.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG83.LWG83R1
   then
        rm ${reportdir}/ULWG83.LWG83R1
fi
if test -a ${reportdir}/ULWG83.LWG83R2
   then
        rm ${reportdir}/ULWG83.LWG83R2
fi
if test -a ${reportdir}/ULWG83.LWG83R3
   then
        rm ${reportdir}/ULWG83.LWG83R3
fi
if test -a ${reportdir}/ULWG83.LWG83R4
   then
        rm ${reportdir}/ULWG83.LWG83R4
fi
if test -a ${reportdir}/ULWG83.LWG83R5
   then
        rm ${reportdir}/ULWG83.LWG83R5
fi
if test -a ${reportdir}/ULWG83.LWG83R6
   then
        rm ${reportdir}/ULWG83.LWG83R6
fi
if test -a ${reportdir}/ULWG83.LWG83RZ
   then
        rm ${reportdir}/ULWG83.LWG83RZ
fi
# run the program

sas ${codedir}/UTLWG83.sas -log ${reportdir}/ULWG83.LWG83R1  -mautosource
