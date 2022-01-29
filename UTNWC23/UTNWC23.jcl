#UTNWC16.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWC23.NWC23R1
then
rm ${reportdir}/UNWC23.NWC23R1
fi
if test -a ${reportdir}/UNWC23.NWC23RZ
then
rm ${reportdir}/UNWC23.NWC23RZ
fi
if test -a ${reportdir}/UNWC23.NWC23R2
then
rm ${reportdir}/UNWC23.NWC23R2
fi

# run the program

sas ${codedir}/UTNWC23.sas -log ${reportdir}/UNWC23.NWC23R1  -mautosource
