#UTLWS06.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS06.LWS06R1
   then
        rm ${reportdir}/ULWS06.LWS06R1
fi
if test -a ${reportdir}/ULWS06.LWS06R2
   then
        rm ${reportdir}/ULWS06.LWS06R2
fi
if test -a ${reportdir}/ULWS06.LWS06R5
   then
        rm ${reportdir}/ULWS06.LWS06R5
fi
if test -a ${reportdir}/ULWS06.LWS06R7
   then
        rm ${reportdir}/ULWS06.LWS06R7
fi
if test -a ${reportdir}/ULWS06.LWS06R8
   then
        rm ${reportdir}/ULWS06.LWS06R8
fi
if test -a ${reportdir}/ULWS06.LWS06R9
   then
        rm ${reportdir}/ULWS06.LWS06R9
fi
if test -a ${reportdir}/ULWS06.LWS06R10
   then
        rm ${reportdir}/ULWS06.LWS06R10
fi
if test -a ${reportdir}/ULWS06.LWS06R11
   then
        rm ${reportdir}/ULWS06.LWS06R11
fi
if test -a ${reportdir}/ULWS06.LWS06R12
   then
        rm ${reportdir}/ULWS06.LWS06R12
fi
if test -a ${reportdir}/ULWS06.LWS06R13
   then
        rm ${reportdir}/ULWS06.LWS06R13
fi
if test -a ${reportdir}/ULWS06.LWS06R14
   then
        rm ${reportdir}/ULWS06.LWS06R14
fi
# run the program

sas ${codedir}/UTLWS06.sas -log ${reportdir}/ULWS06.LWS06R1  -mautosource
