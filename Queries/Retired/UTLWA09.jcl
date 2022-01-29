#UTLWA09.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA09.LWA09R1
then
rm ${reportdir}/ULWA09.LWA09R1
fi
if test -a ${reportdir}/ULWA09.LWA09R2
then
rm ${reportdir}/ULWA09.LWA09R2
fi
if test -a ${reportdir}/ULWA09.LWA09R3
then
rm ${reportdir}/ULWA09.LWA09R3
fi
if test -a ${reportdir}/ULWA09.LWA09R4
then
rm ${reportdir}/ULWA09.LWA09R4
fi
if test -a ${reportdir}/ULWA09.LWA09RZ
then
rm ${reportdir}/ULWA09.LWA09RZ
fi

# run the program

sas ${codedir}/UTLWA09.sas -log ${reportdir}/ULWA09.LWA09R1  -mautosource
