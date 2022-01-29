#UTLWG81.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG81.LWG81R1
then
rm ${reportdir}/ULWG81.LWG81R1
fi
if test -a ${reportdir}/ULWG81.LWG81R2
then
rm ${reportdir}/ULWG81.LWG81R2
fi
if test -a ${reportdir}/ULWG81.LWG81RZ
then
rm ${reportdir}/ULWG81.LWG81RZ
fi

# run the program

sas ${codedir}/UTLWG81.sas -log ${reportdir}/ULWG81.LWG81R1  -mautosource
