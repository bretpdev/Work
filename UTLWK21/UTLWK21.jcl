#UTLWK21.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK21.LWK21R1
then
rm ${reportdir}/ULWK21.LWK21R1
fi
if test -a ${reportdir}/ULWK21.LWK21R2
then
rm ${reportdir}/ULWK21.LWK21R2
fi
if test -a ${reportdir}/ULWK21.LWK21RZ
then
rm ${reportdir}/ULWK21.LWK21RZ
fi

# run the program

sas ${codedir}/UTLWK21.sas -log ${reportdir}/ULWK21.LWK21R1  -mautosource
