#UTLWQ10.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ10.LWQ10R1
   then
        rm ${reportdir}/ULWQ10.LWQ10R1
fi
if test -a ${reportdir}/ULWQ10.LWQ10R2
   then
        rm ${reportdir}/ULWQ10.LWQ10R2
fi
if test -a ${reportdir}/ULWQ10.LWQ10R3
   then
        rm ${reportdir}/ULWQ10.LWQ10R3
fi
if test -a ${reportdir}/ULWQ10.LWQ10R4
   then
        rm ${reportdir}/ULWQ10.LWQ10R4
fi
if test -a ${reportdir}/ULWQ10.LWQ10RZ
   then
        rm ${reportdir}/ULWQ10.LWQ10RZ
fi
# run the program

sas ${codedir}/UTLWQ10.sas -log ${reportdir}/ULWQ10.LWQ10R1  -mautosource
