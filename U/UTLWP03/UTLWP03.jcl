#UTLWP03.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWP03.LWP03R1
then
rm ${reportdir}/ULWP03.LWP03R1
fi
if test -a ${reportdir}/ULWP03.LWP03R2
then
rm ${reportdir}/ULWP03.LWP03R2
fi
if test -a ${reportdir}/ULWP03.LWP03R3
then
rm ${reportdir}/ULWP03.LWP03R3
fi
if test -a ${reportdir}/ULWP03.LWP03RZ
then
rm ${reportdir}/ULWP03.LWP03RZ
fi

# run the program

sas ${codedir}/UTLWP03.sas -log ${reportdir}/ULWP03.LWP03R1  -mautosource
