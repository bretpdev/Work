#UTLWG80.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG80.LWG80R1
   then
        rm ${reportdir}/ULWG80.LWG80R1
fi
if test -a ${reportdir}/ULWG80.LWG80R2
   then
        rm ${reportdir}/ULWG80.LWG80R2
fi
if test -a ${reportdir}/ULWG80.LWG80R3
   then
        rm ${reportdir}/ULWG80.LWG80R3
fi
if test -a ${reportdir}/ULWG80.LWG80R4
   then
        rm ${reportdir}/ULWG80.LWG80R4
fi
if test -a ${reportdir}/ULWG80.LWG80R5
   then
        rm ${reportdir}/ULWG80.LWG80R5
fi
if test -a ${reportdir}/ULWG80.LWG80R6
   then
        rm ${reportdir}/ULWG80.LWG80R6
fi
if test -a ${reportdir}/ULWG80.LWG80R7
   then
        rm ${reportdir}/ULWG80.LWG80R7
fi
if test -a ${reportdir}/ULWG80.LWG80R8
   then
        rm ${reportdir}/ULWG80.LWG80R8
fi
if test -a ${reportdir}/ULWG80.LWG80R9
   then
        rm ${reportdir}/ULWG80.LWG80R9
fi
if test -a ${reportdir}/ULWG80.LWG80R10
   then
        rm ${reportdir}/ULWG80.LWG80R10
fi
if test -a ${reportdir}/ULWG80.LWG80R11
   then
        rm ${reportdir}/ULWG80.LWG80R11
fi
if test -a ${reportdir}/ULWG80.LWG80R12
   then
        rm ${reportdir}/ULWG80.LWG80R12
fi
if test -a ${reportdir}/ULWG80.LWG80R13
   then
        rm ${reportdir}/ULWG80.LWG80R13
fi
if test -a ${reportdir}/ULWG80.LWG80RZ
   then
        rm ${reportdir}/ULWG80.LWG80RZ
fi
# run the program

sas ${codedir}/UTLWG80.sas -log ${reportdir}/ULWG80.LWG80R1  -mautosource
