#UTLWR23.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR23.LWR23R1
then
rm ${reportdir}/ULWR23.LWR23R1
fi
if test -a ${reportdir}/ULWR23.LWR23R2
then
rm ${reportdir}/ULWR23.LWR23R2
fi
if test -a ${reportdir}/ULWR23.LWR23R3
then
rm ${reportdir}/ULWR23.LWR23R3
fi
if test -a ${reportdir}/ULWR23.LWR23R4
then
rm ${reportdir}/ULWR23.LWR23R4
fi
if test -a ${reportdir}/ULWR23.LWR23R5
then
rm ${reportdir}/ULWR23.LWR23R5
fi
if test -a ${reportdir}/ULWR23.LWR23R6
then
rm ${reportdir}/ULWR23.LWR23R6
fi
if test -a ${reportdir}/ULWR23.LWR23R7
then
rm ${reportdir}/ULWR23.LWR23R7
fi
if test -a ${reportdir}/ULWR23.LWR23RZ
then
rm ${reportdir}/ULWR23.LWR23RZ
fi

# run the program

sas ${codedir}/UTLWR23.sas -log ${reportdir}/ULWR23.LWR23R1  -mautosource
