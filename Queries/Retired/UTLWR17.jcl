#UTLWR17.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR17.LWR17R1
then
rm ${reportdir}/ULWR17.LWR17R1
fi
if test -a ${reportdir}/ULWR17.LWR17R2
then
rm ${reportdir}/ULWR17.LWR17R2
fi
if test -a ${reportdir}/ULWR17.LWR17RZ
then
rm ${reportdir}/ULWR17.LWR17RZ
fi

# run the program

sas ${codedir}/UTLWR17.sas -log ${reportdir}/ULWR17.LWR17R1  -mautosource
