#UTLWR22.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR22.LWR22R1
then
rm ${reportdir}/ULWR22.LWR22R1
fi
if test -a ${reportdir}/ULWR22.LWR22R2
then
rm ${reportdir}/ULWR22.LWR22R2
fi
if test -a ${reportdir}/ULWR22.LWR22R3
then
rm ${reportdir}/ULWR22.LWR22R3
fi
if test -a ${reportdir}/ULWR22.LWR22RZ
then
rm ${reportdir}/ULWR22.LWR22RZ
fi

# run the program

sas ${codedir}/UTLWR22.sas -log ${reportdir}/ULWR22.LWR22R1  -mautosource
