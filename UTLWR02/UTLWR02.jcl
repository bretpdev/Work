#UTLWR02.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR02.LWR02R1
then
rm ${reportdir}/ULWR02.LWR02R1
fi
if test -a ${reportdir}/ULWR02.LWR02R2
then
rm ${reportdir}/ULWR02.LWR02R2
fi
if test -a ${reportdir}/ULWR02.LWR02RZ
then
rm ${reportdir}/ULWR02.LWR02RZ
fi

# run the program

sas ${codedir}/UTLWR02.sas -log ${reportdir}/ULWR02.LWR02R1  -mautosource
