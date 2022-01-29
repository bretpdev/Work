#UTLWO99.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO99.LWO99R1
then
rm ${reportdir}/ULWO99.LWO99R1
fi
if test -a ${reportdir}/ULWO99.LWO99R2
then
rm ${reportdir}/ULWO99.LWO99R2
fi
if test -a ${reportdir}/ULWO99.LWO99RZ
then
rm ${reportdir}/ULWO99.LWO99RZ
fi

# run the program

sas ${codedir}/UTLWO99.sas -log ${reportdir}/ULWO99.LWO99R1  -mautosource
