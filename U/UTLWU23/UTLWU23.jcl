#UTLWU23.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU23.LWU23R1
then
rm ${reportdir}/ULWU23.LWU23R1
fi
if test -a ${reportdir}/ULWU23.LWU23R2
then
rm ${reportdir}/ULWU23.LWU23R2
fi
if test -a ${reportdir}/ULWU23.LWU23R3
then
rm ${reportdir}/ULWU23.LWU23R3
fi
if test -a ${reportdir}/ULWU23.LWU23R4
then
rm ${reportdir}/ULWU23.LWU23R4
fi
if test -a ${reportdir}/ULWU23.LWU23RZ
then
rm ${reportdir}/ULWU23.LWU23RZ
fi

# run the program

sas ${codedir}/UTLWU23.sas -log ${reportdir}/ULWU23.LWU23R1  -mautosource
