#UTLWU03.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU03.LWU03R1
then
rm ${reportdir}/ULWU03.LWU03R1
fi
if test -a ${reportdir}/ULWU03.LWU03R2
then
rm ${reportdir}/ULWU03.LWU03R2
fi
if test -a ${reportdir}/ULWU03.LWU03RZ
then
rm ${reportdir}/ULWU03.LWU03RZ
fi

# run the program

sas ${codedir}/UTLWU03.sas -log ${reportdir}/ULWU03.LWU03R1  -mautosource
