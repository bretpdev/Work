#UTNWT01.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWT01.NWT01R1
then
rm ${reportdir}/UNWT01.NWT01R1
fi
if test -a ${reportdir}/UNWT01.NWT01RZ
then
rm ${reportdir}/UNWT01.NWT01RZ
fi
if test -a ${reportdir}/UNWT01.NWT01R2
then
rm ${reportdir}/UNWT01.NWT01R2
fi
if test -a ${reportdir}/UNWT01.NWT01R3
then
rm ${reportdir}/UNWT01.NWT01R3
fi
if test -a ${reportdir}/UNWT01.NWT01R4
then
rm ${reportdir}/UNWT01.NWT01R4
fi
if test -a ${reportdir}/UNWT01.NWT01R5
then
rm ${reportdir}/UNWT01.NWT01R5
fi
if test -a ${reportdir}/UNWT01.NWT01R6
then
rm ${reportdir}/UNWT01.NWT01R6
fi
if test -a ${reportdir}/UNWT01.NWT01R7
then
rm ${reportdir}/UNWT01.NWT01R7
fi
if test -a ${reportdir}/UNWT01.NWT01R8
then
rm ${reportdir}/UNWT01.NWT01R8
fi
if test -a ${reportdir}/UNWT01.NWT01R9
then
rm ${reportdir}/UNWT01.NWT01R9
fi
if test -a ${reportdir}/UNWT01.NWT01R10
then
rm ${reportdir}/UNWT01.NWT01R10
fi
if test -a ${reportdir}/UNWT01.NWT01R11
then
rm ${reportdir}/UNWT01.NWT01R11
fi
if test -a ${reportdir}/UNWT01.NWT01R12
then
rm ${reportdir}/UNWT01.NWT01R12
fi
if test -a ${reportdir}/UNWT01.NWT01R13
then
rm ${reportdir}/UNWT01.NWT01R13
fi
if test -a ${reportdir}/UNWT01.NWT01R14
then
rm ${reportdir}/UNWT01.NWT01R14
fi
if test -a ${reportdir}/UNWT01.NWT01R15
then
rm ${reportdir}/UNWT01.NWT01R15
fi
if test -a ${reportdir}/UNWT01.NWT01R16
then
rm ${reportdir}/UNWT01.NWT01R16
fi
if test -a ${reportdir}/UNWT01.NWT01R17
then
rm ${reportdir}/UNWT01.NWT01R17
fi

# run the program

sas ${codedir}/UTNWT01.sas -log ${reportdir}/UNWT01.NWT01R1  -mautosource
