#UTNWF09.jcl Monthly CornerStone Bankruptcy Aging Data - FED

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWF09.NWF09R1
then
rm ${reportdir}/UNWF09.NWF09R1
fi
if test -a ${reportdir}/UNWF09.NWF09R2
then
rm ${reportdir}/UNWF09.NWF09R2
fi
if test -a ${reportdir}/UNWF09.NWF09R3
then
rm ${reportdir}/UNWF09.NWF09R3
fi
if test -a ${reportdir}/UNWF09.NWF09R4
then
rm ${reportdir}/UNWF09.NWF09R4
fi
if test -a ${reportdir}/UNWF09.NWF09R5
then
rm ${reportdir}/UNWF09.NWF09R5
fi

# run the program

sas ${codedir}/UTNWF09.sas -log ${reportdir}/UNWF09.NWF09R1  -mautosource
