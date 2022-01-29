#UTLWG12.jcl  esign letters
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG12.LWG12R1
   then
        rm ${reportdir}/ULWG12.LWG12R1
fi
if test -a ${reportdir}/ULWG12.LWG12RZ
   then
        rm ${reportdir}/ULWG12.LWG12RZ
fi
if test -a ${reportdir}/ULWG12.LWG12R2
   then
        rm ${reportdir}/ULWG12.LWG12R2
fi
if test -a ${reportdir}/ULWG12.LWG12R3
   then
        rm ${reportdir}/ULWG12.LWG12R3
fi
if test -a ${reportdir}/ULWG12.LWG12R4
   then
        rm ${reportdir}/ULWG12.LWG12R4
fi
if test -a ${reportdir}/ULWG12.LWG12R5
   then
        rm ${reportdir}/ULWG12.LWG12R5
fi
if test -a ${reportdir}/ULWG12.LWG12R6
   then
        rm ${reportdir}/ULWG12.LWG12R6
fi
if test -a ${reportdir}/ULWG12.LWG12R8
   then
        rm ${reportdir}/ULWG12.LWG12R8
fi
if test -a ${reportdir}/ULWG12.LWG12R9
   then
        rm ${reportdir}/ULWG12.LWG12R9
fi
if test -a ${reportdir}/ULWG12.LWG12R10
   then
        rm ${reportdir}/ULWG12.LWG12R10
fi
if test -a ${reportdir}/ULWG12.LWG12R11
   then
        rm ${reportdir}/ULWG12.LWG12R11
fi
if test -a ${reportdir}/ULWG12.LWG12R12
   then
        rm ${reportdir}/ULWG12.LWG12R12
fi
if test -a ${reportdir}/ULWG12.LWG12R13
   then
        rm ${reportdir}/ULWG12.LWG12R13
fi
if test -a ${reportdir}/ULWG12.LWG12R14
   then
        rm ${reportdir}/ULWG12.LWG12R14
fi
if test -a ${reportdir}/ULWG12.LWG12R15
   then
        rm ${reportdir}/ULWG12.LWG12R15
fi
if test -a ${reportdir}/ULWG12.LWG12R16
   then
        rm ${reportdir}/ULWG12.LWG12R16
fi
if test -a ${reportdir}/ULWG12.LWG12R17
   then
        rm ${reportdir}/ULWG12.LWG12R17
fi
if test -a ${reportdir}/ULWG12.LWG12R18
   then
        rm ${reportdir}/ULWG12.LWG12R18
fi
if test -a ${reportdir}/ULWG12.LWG12R19
   then
        rm ${reportdir}/ULWG12.LWG12R19
fi
if test -a ${reportdir}/ULWG12.LWG12R20
   then
        rm ${reportdir}/ULWG12.LWG12R20
fi
if test -a ${reportdir}/ULWG12.LWG12R22
   then
        rm ${reportdir}/ULWG12.LWG12R22
fi
if test -a ${reportdir}/ULWG12.LWG12R23
   then
        rm ${reportdir}/ULWG12.LWG12R23
fi
if test -a ${reportdir}/ULWG12.LWG12R26
   then
        rm ${reportdir}/ULWG12.LWG12R26
fi
if test -a ${reportdir}/ULWG12.LWG12R27
   then
        rm ${reportdir}/ULWG12.LWG12R27
fi
if test -a ${reportdir}/ULWG12.LWG12R28
   then
        rm ${reportdir}/ULWG12.LWG12R28
fi
if test -a ${reportdir}/ULWG12.LWG12R29
   then
        rm ${reportdir}/ULWG12.LWG12R29
fi
if test -a ${reportdir}/ULWG12.LWG12R99
   then
        rm ${reportdir}/ULWG12.LWG12R99
fi

# run the program

sas ${codedir}/UTLWG12.sas -log ${reportdir}/ULWG12.LWG12R1  -mautosource
