#UTLWA18.jcl Special Write off Program - Final List
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA18.LWA18R1
then
rm ${reportdir}/ULWA18.LWA18R1
fi
if test -a ${reportdir}/ULWA18.LWA18RZ
then
rm ${reportdir}/ULWA18.LWA18RZ
fi
if test -a ${reportdir}/ULWA18.LWA18R2
then
rm ${reportdir}/ULWA18.LWA18R2
fi
if test -a ${reportdir}/ULWA18.LWA18R3
then
rm ${reportdir}/ULWA18.LWA18R3
fi
if test -a ${reportdir}/ULWA18.LWA18R4
then
rm ${reportdir}/ULWA18.LWA18R4
fi
if test -a ${reportdir}/ULWA18.LWA18R5
then
rm ${reportdir}/ULWA18.LWA18R5
fi

# run the program

sas ${codedir}/UTLWA18.sas -log ${reportdir}/ULWA18.LWA18R1  -mautosource
