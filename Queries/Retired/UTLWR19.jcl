#UTLWR19.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR19.LWR19R1
then
rm ${reportdir}/ULWR19.LWR19R1
fi
if test -a ${reportdir}/ULWR19.LWR19R2
then
rm ${reportdir}/ULWR19.LWR19R2
fi
if test -a ${reportdir}/ULWR19.LWR19RZ
then
rm ${reportdir}/ULWR19.LWR19RZ
fi

# run the program

sas ${codedir}/UTLWR19.sas -log ${reportdir}/ULWR19.LWR19R1  -mautosource
