#UTLWK18.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK18.LWK18R1
then
rm ${reportdir}/ULWK18.LWK18R1
fi
if test -a ${reportdir}/ULWK18.LWK18R2
then
rm ${reportdir}/ULWK18.LWK18R2
fi
if test -a ${reportdir}/ULWK18.LWK18RZ
then
rm ${reportdir}/ULWK18.LWK18RZ
fi

# run the program

sas ${codedir}/UTLWK18.sas -log ${reportdir}/ULWK18.LWK18R1  -mautosource
