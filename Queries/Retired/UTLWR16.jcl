#UTLWR16.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR16.LWR16R1
then
rm ${reportdir}/ULWR16.LWR16R1
fi
if test -a ${reportdir}/ULWR16.LWR16R2
then
rm ${reportdir}/ULWR16.LWR16R2
fi
if test -a ${reportdir}/ULWR16.LWR16RZ
then
rm ${reportdir}/ULWR16.LWR16RZ
fi

# run the program

sas ${codedir}/UTLWR16.sas -log ${reportdir}/ULWR16.LWR16R1  -mautosource
