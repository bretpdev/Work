#UTLWG84.jcl  esign REMINDER letters
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG84.LWG84R1
   then
        rm ${reportdir}/ULWG84.LWG84R1
fi
if test -a ${reportdir}/ULWG84.LWG84RZ
   then
        rm ${reportdir}/ULWG84.LWG84RZ
fi
if test -a ${reportdir}/ULWG84.LWG84R2
   then
        rm ${reportdir}/ULWG84.LWG84R2
fi
if test -a ${reportdir}/ULWG84.LWG84R3
   then
        rm ${reportdir}/ULWG84.LWG84R3
fi
if test -a ${reportdir}/ULWG84.LWG84R4
   then
        rm ${reportdir}/ULWG84.LWG84R4
fi
if test -a ${reportdir}/ULWG84.LWG84R5
   then
        rm ${reportdir}/ULWG84.LWG84R5
fi
if test -a ${reportdir}/ULWG84.LWG84R6
   then
        rm ${reportdir}/ULWG84.LWG84R6
fi
if test -a ${reportdir}/ULWG84.LWG84R7
   then
        rm ${reportdir}/ULWG84.LWG84R7
fi
if test -a ${reportdir}/ULWG84.LWG84R8
   then
        rm ${reportdir}/ULWG84.LWG84R8
fi
if test -a ${reportdir}/ULWG84.LWG84R9
   then
        rm ${reportdir}/ULWG84.LWG84R9
fi

# run the program

sas ${codedir}/UTLWG84.sas -log ${reportdir}/ULWG84.LWG84R1  -mautosource
