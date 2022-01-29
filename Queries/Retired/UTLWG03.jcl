#UTLWG03.jcl  Monthly Guaranty Reporting
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG03.LWG03RZ
   then
        rm ${reportdir}/ULWG03.LWG03RZ
fi
if test -a ${reportdir}/ULWG03.LWG03R1
   then
        rm ${reportdir}/ULWG03.LWG03R1
fi
if test -a ${reportdir}/ULWG03.LWG03R2
   then
        rm ${reportdir}/ULWG03.LWG03R2
fi
if test -a ${reportdir}/ULWG03.LWG03R3
   then
        rm ${reportdir}/ULWG03.LWG03R3
fi
if test -a ${reportdir}/ULWG03.LWG03R4
   then
        rm ${reportdir}/ULWG03.LWG03R4
fi
if test -a ${reportdir}/ULWG03.LWG03R5
   then
        rm ${reportdir}/ULWG03.LWG03R5
fi
if test -a ${reportdir}/ULWG03.LWG03R6
   then
        rm ${reportdir}/ULWG03.LWG03R6
fi
if test -a ${reportdir}/ULWG03.LWG03R20
   then
        rm ${reportdir}/ULWG03.LWG03R20
fi
if test -a ${reportdir}/ULWG03.LWG03R30
   then
        rm ${reportdir}/ULWG03.LWG03R30
fi
if test -a ${reportdir}/ULWG03.LWG03R31
   then
        rm ${reportdir}/ULWG03.LWG03R31
fi
if test -a ${reportdir}/ULWG03.LWG03R32
   then
        rm ${reportdir}/ULWG03.LWG03R32
fi
if test -a ${reportdir}/ULWG03.LWG03R33
   then
        rm ${reportdir}/ULWG03.LWG03R33
fi
if test -a ${reportdir}/ULWG03.LWG03R34
   then
        rm ${reportdir}/ULWG03.LWG03R34
fi
if test -a ${reportdir}/ULWG03.LWG03R35
   then
        rm ${reportdir}/ULWG03.LWG03R35
fi
if test -a ${reportdir}/ULWG03.LWG03R36
   then
        rm ${reportdir}/ULWG03.LWG03R36
fi
if test -a ${reportdir}/ULWG03.LWG03R37
   then
        rm ${reportdir}/ULWG03.LWG03R37
fi

# run the program

sas ${codedir}/UTLWG03.sas -log ${reportdir}/ULWG03.LWG03R1  -mautosource
